using UnityEngine;

public class gunslinger : MonoBehaviour
{
    private SimpleShoot simpleShoot;
    public float damage = 10f;
    public float range = 100f;
    private OVRGrabbable ovrGrabbable;
    public OVRInput.Button shootingButton;
    public OVRInput.Button secondshootingButton;
    public Camera fpsCam;
    private void Start() {
        simpleShoot = GetComponent<SimpleShoot>();
        ovrGrabbable = GetComponent<OVRGrabbable>();
    
    }

    void Update()
    {
        if(ovrGrabbable.isGrabbed && OVRInput.GetDown(shootingButton) || ovrGrabbable.isGrabbed && OVRInput.GetDown(secondshootingButton))
        {
            simpleShoot.TriggerShoot();
            ShootRay();
            Debug.Log("shooting!");
        }
    }

    void ShootRay ()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
