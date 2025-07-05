using System.Collections.Generic;
using UnityEngine;
using YG;


public class PlayerSkinController : MonoBehaviour
{
    [SerializeField] private GameObject[] _topMeshes;
    [SerializeField] private GameObject[] _middleMeshes;
    [SerializeField] private GameObject[] _bottomMeshes;
    [SerializeField] private GameObject[] _footMeshes;
    [SerializeField] private GameObject[] _suitMeshes;
    [SerializeField] private GameObject[] _trails;

    private Dictionary<CustomItemType, GameObject[]> _meshMap;

    private void Awake()
    {
        CustomizationWindow.onUpdateShop += ApplyEquippedItems;

        _meshMap = new Dictionary<CustomItemType, GameObject[]>
        {
            { CustomItemType.Top, _topMeshes },
            { CustomItemType.Middle, _middleMeshes },
            { CustomItemType.Bottom, _bottomMeshes },
            { CustomItemType.Foot, _footMeshes },
            { CustomItemType.Suit, _suitMeshes },
            { CustomItemType.Trail, _trails}
        };

        ApplyEquippedItems();
    }

    private void OnDestroy()
    {
        CustomizationWindow.onUpdateShop -= ApplyEquippedItems;
    }

    public void ApplyEquippedItems()
    {
        var equipped = YG2.saves.GetEquipedItems();

        equipped.TryGetValue(CustomItemType.Suit.ToString(), out int suitIndex);

        if (suitIndex != 0)
        {
            foreach (var kvp in _meshMap)
            {
                if (kvp.Key == CustomItemType.Suit)
                    EnableMesh(kvp.Value, suitIndex);
                else
                {
                    equipped.TryGetValue(kvp.Key.ToString(), out int itemIndex);
                    EnableMesh(kvp.Value, itemIndex);
                }
            }
            EnableMesh(_middleMeshes, -1);
            EnableMesh(_bottomMeshes, -1);
            EnableMesh(_footMeshes, -1);
        }
        else
        {
            foreach (var kvp in _meshMap)
            {
                if (kvp.Key == CustomItemType.Suit)
                {
                    EnableMesh(kvp.Value, 0); 
                    continue;
                }

                equipped.TryGetValue(kvp.Key.ToString(), out int itemIndex);
                EnableMesh(kvp.Value, itemIndex);
            }
        }
    }

    private void EnableMesh(GameObject[] meshes, int index)
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            if (meshes[i] != null)
            {
                if(index == -1)
                    meshes[i].SetActive(false);
                else
                  meshes[i].SetActive(i == index);
            }
        }
    }
}
