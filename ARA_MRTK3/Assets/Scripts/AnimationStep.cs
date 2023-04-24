
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class AnimationStep
    {
        public int frame = 0;
        public string description = "Step Title";
        public List<MeshRenderer> partsToHighlight = new List<MeshRenderer>();
        
        public void HighlightParts(Material highlightMaterial, Dictionary<MeshRenderer, Material> originalMaterials)
        {
            foreach (var part in partsToHighlight)
            {
                if (!originalMaterials.ContainsKey(part))
                {
                    originalMaterials[part] = part.material;
                }
                part.material = highlightMaterial;
            }
        }
        
    }