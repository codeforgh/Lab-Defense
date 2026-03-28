
using UnityEngine;
using UnityEngine.EventSystems;

public class HexSlot : MonoBehaviour, IDropHandler
{
    public bool isOccupied = false;

    public void OnDrop(PointerEventData eventData) {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem item = dropped.GetComponent<DraggableItem>();

        if (item != null && !isOccupied) {
            GameObject newTurret = Instantiate(item.buildingPrefab, transform.position, Quaternion.identity);
        
            // This is the important part:
            newTurret.transform.SetParent(this.transform);
        
            // Option A: Reset to 1,1,1 (If the slot scale is 1,1,1)
            //newTurret.transform.localScale = Vector3.one; 
        
            // Option B: If it's still too small, you can manually set a bigger scale
             newTurret.transform.localScale = new Vector3(10f, 10f, 10f); 

            newTurret.transform.localPosition = Vector3.zero;
            isOccupied = true;
        }
    }
}