/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BOC.BTagged.BTagged;

namespace BOC.BTagged
{
    [CreateAssetMenu(fileName = "TagQuery", menuName = "BTagged/Tag Query")]
    public class TagQuery : BTaggedSO
    {
        public TagWithRule[] matchingTags;
        public TagQueryWithTarget[] subQueries;
    }
}
