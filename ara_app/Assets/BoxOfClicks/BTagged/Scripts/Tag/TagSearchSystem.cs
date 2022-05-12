/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

#if UNITY_ENTITIES
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using static BOC.BTagged.BTagged;
using Unity.Jobs;

namespace BOC.BTagged
{
    [DisableAutoCreation]
    [BurstCompile]
    public class TagSearchSystem : SystemBase
    {
        static void GatherNestedChildrenWithTags(Entity baseEntity, ref NativeList<Entity> childLlist, ref ComponentDataFromEntity<TagICD> TagICD_CDFE, ref BufferFromEntity<Child> Child_BFE, ref BufferFromEntity<LinkedEntityGroup> LEG_BFE)
        {
            if(TagICD_CDFE.HasComponent(baseEntity) && !childLlist.Contains(baseEntity)) childLlist.Add(baseEntity);
#if UNITY_2020_1_OR_NEWER
            if (Child_BFE.HasComponent(baseEntity))
#else
            if (Child_BFE.Exists(baseEntity))
#endif
            {
                var children = Child_BFE[baseEntity];
                for (int c = 0; c < children.Length; ++c)
                {
                    GatherNestedChildrenWithTags(children[c].Value, ref childLlist, ref TagICD_CDFE, ref Child_BFE, ref LEG_BFE);
                }
            }
#if UNITY_2020_1_OR_NEWER
            else if (LEG_BFE.HasComponent(baseEntity))
#else
            else if (LEG_BFE.Exists(baseEntity))
#endif
            {
                var children = LEG_BFE[baseEntity];
                for (int c = 0; c < children.Length; ++c)
                {
                    if (children[c].Value != baseEntity) GatherNestedChildrenWithTags(children[c].Value, ref childLlist, ref TagICD_CDFE, ref Child_BFE, ref LEG_BFE);
                }
            }
        }



        public bool RespectHierarchy = true;
        public bool FindAll = true;
        public bool LastGroup = false;
        public bool EntityFirstSearch = false;
        public Entity ParentEntity;
        [ReadOnly] public NativeArray<TagHashWithRules> MatchingHashes;
        [WriteOnly] public NativeList<Entity> Results;

        
        protected override void OnUpdate()
        {
            bool respectHierarchy = RespectHierarchy;
            bool findAll = FindAll;
            bool lastGroup = LastGroup;
            NativeArray<TagHashWithRules> hashes = MatchingHashes;
            NativeList<Entity> results = Results;
            Entity parentEntity = ParentEntity;

            if (EntityFirstSearch)
            {
                BufferFromEntity<Child> Child_BFE = GetBufferFromEntity<Child>(true);
                BufferFromEntity<LinkedEntityGroup> LEG_BFE = GetBufferFromEntity<LinkedEntityGroup>(true);
                ComponentDataFromEntity<TagICD> TagICD_CDFE = GetComponentDataFromEntity<TagICD>(true);
                Job
                    .WithBurst()
                    .WithReadOnly(TagICD_CDFE)
                    .WithReadOnly(Child_BFE)
                    .WithReadOnly(LEG_BFE)
                    .WithCode(() =>
                {
                    if (respectHierarchy && hashes[0].searchOption != Search.Target)
                    {
                        GatherNestedChildrenWithTags(parentEntity, ref results, ref TagICD_CDFE, ref Child_BFE, ref LEG_BFE);
                    }
                    else
                    {
                        results.Add(parentEntity);
                    }

                    for (int r = results.Length - 1; r >= 0; --r)
                    {
                        bool matched = false;
                        if (results[r] != parentEntity || hashes[0].searchOption != Search.Children)
                        {
                            ref var entityHashes = ref TagICD_CDFE[results[r]].Blob.Value.hashes;
                            matched = hashes.Match(ref entityHashes);
                        }
                        if (!matched) results.RemoveAtSwapBack(r);
                    }
                }).Run();
            }
            else
            {
                Entities
                    .WithBurst()
                    .ForEach((Entity e, in TagICD tagICD) =>
                {
                    if (!findAll && results.Length > 0 && (!respectHierarchy || lastGroup) ) return;

                    ref var entityHashes = ref tagICD.Blob.Value.hashes;
                    bool matched = hashes.Match(ref entityHashes);
                    if (matched) results.Add(e);
                }).Run();

                if (respectHierarchy)
                {
                    Job
                        .WithBurst()
                        .WithCode(() =>
                    {
                        for (int r = results.Length - 1; r >= 0; --r)
                        {
                            bool isChild = false;
                            Entity e = results[r];
                            // If respecting hierarchy, iterate up hierarchy to see if object is child of base entity
                            while (HasComponent<Parent>(e) && !isChild)
                            {
                                Entity p = GetComponent<Parent>(e).Value;
                                if (p == parentEntity) isChild = true;
                                e = p;
                            }
                            while (HasComponent<PreviousParent>(e) && !isChild)
                            {
                                Entity p = GetComponent<PreviousParent>(e).Value;
                                if (p == parentEntity) isChild = true;
                                e = p;
                            }
                            if (!isChild) results.RemoveAtSwapBack(r);
                        }
                    }).Run();
                }
            }
        }
    }
}
#endif