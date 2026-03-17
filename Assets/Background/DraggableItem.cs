using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject buildingPrefab; // The 3D/2D turret to place in the world
    [HideInInspector] public Transform parentAfterDrag;
    private CanvasGroup canvasGroup;

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root); // Pull it out of the bottom bar
        transform.SetAsLastSibling();        // Ensure it stays on top of all UI
        canvasGroup.blocksRaycasts = false;  // IMPORTANT: Allows the mouse to "see through" the icon to the slot
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition; // Follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentAfterDrag);
        canvasGroup.blocksRaycasts = true; // Let the mouse interact with it again
    }
}