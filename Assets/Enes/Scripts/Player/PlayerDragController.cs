using UnityEngine;

public class PlayerDragController : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float dragForce;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float spinSpeed;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject uiCameraControl;

    private Camera mainCam;

    private Animator playerAnim;

    private GameObject heldObject;
    private Rigidbody heldObjectRb;

    private LayerMask draggableLayer;
    public float rotationSpeed = 10f;
    private bool isPickedUp;
    public bool IsPickedUp { get { return isPickedUp; } }
    private void Awake()
    {
        mainCam = Camera.main;
        draggableLayer = LayerMask.GetMask("Draggable");
        playerAnim = GetComponent<Animator>();
        uiPanel.SetActive(false);
        uiCameraControl.SetActive(false);
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            // Is an object currently being hold?
            if (heldObject == null)
            {
                Ray cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(cameraRay, out RaycastHit hit, float.MaxValue, draggableLayer))
                {
                    PickupObject(hit.transform.gameObject);
                }

            }
        }
        // Is an object currently being hold?
        if (heldObject != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                ReleaseObject();
            }
            else
            {
                MoveObject();


                if (Input.GetKey(KeyCode.Q))
                {
                    RotateObject(1);
                }
                // Right Rotate
                if (Input.GetKey(KeyCode.E))
                {
                    RotateObject(-1);
                }
                if (Input.GetKey(KeyCode.C))
                {
                    SlideObject(1);
                }
                if (Input.GetKey(KeyCode.Z))
                {
                    SlideObject(-1);
                }
                if (Input.GetKey(KeyCode.X))
                {
                    SpinObject(1);
                }
                if (Input.GetKey(KeyCode.V))
                {
                    SpinObject(-1);
                }
            }
        }
    }

    private void PickupObject(GameObject pickedObject)
    {
        if (pickedObject.GetComponent<Rigidbody>())
        {
            heldObjectRb = pickedObject.GetComponent<Rigidbody>();
            heldObjectRb.useGravity = false;
            heldObjectRb.drag = 10;
            heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            heldObjectRb.interpolation = RigidbodyInterpolation.Interpolate;
            heldObjectRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            heldObject = pickedObject;
            heldObject.transform.position = new Vector3(heldObject.transform.position.x, heldObject.transform.position.y, 0);
            isPickedUp = true;

            playerAnim.SetTrigger("dragStart");
            uiPanel.SetActive(true);
        }
        else
        {
            Debug.Log("The object you try to pick up has no Rigidbody! Add it.");
        }
    }

    private void ReleaseObject()
    {
        heldObjectRb.useGravity = true;
        heldObjectRb.drag = 1;
        heldObjectRb.constraints = RigidbodyConstraints.None;

        playerAnim.SetTrigger("dragFinish");
        heldObject = null;
        isPickedUp = false;
        uiPanel.SetActive(false);
    }

    private void MoveObject()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = -mainCam.transform.position.z;

        Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(mouseScreenPosition);

        if (Vector3.Distance(heldObject.transform.position, mouseWorldPosition) > 0.1f)
        {
            Vector3 moveDirection = mouseWorldPosition - heldObject.transform.position;
            heldObjectRb.AddForce(moveDirection * dragForce * Time.deltaTime);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            uiCameraControl.SetActive(true);
        }
        
    }

    private void RotateObject(int direction)
    {
        heldObject.transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime * direction);
    }
    private void SlideObject(int direction)
    {
        heldObject.transform.Rotate(Vector3.up * slideSpeed * Time.deltaTime * direction);

    }
    private void SpinObject(int direction)
    {
        heldObject.transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime * direction);

    }



}
