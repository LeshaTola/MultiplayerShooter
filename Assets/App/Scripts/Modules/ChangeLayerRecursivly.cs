namespace App.Scripts.Modules
{
    using UnityEngine;

    public class ChangeLayerRecursively
    {
        public static void SetLayerRecursively(Transform obj, string layerName)
        { 
            int layer = LayerMask.NameToLayer(layerName);

            // Если слой существует, применяем его
            if (layer != -1)
            {
                SetLayerRecursively(obj, layer);
            }
            else
            {
                Debug.LogError("Слой с именем " + layerName + " не найден!");
            }
        }
        
        public static void SetLayerRecursively(Transform obj, int layer)
        { 
            obj.gameObject.layer = layer;
            foreach (Transform child in obj)
            {
                SetLayerRecursively(child, layer);
            }
            
        }
    }
    
    

}