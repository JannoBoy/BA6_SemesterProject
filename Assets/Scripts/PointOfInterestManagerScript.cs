using UnityEngine;

public class PointOfInterestManagerScript : MonoBehaviour
{
    private MeshRenderer myMesh;

    [SerializeField]
    private Material playerEnteredMaterial;

    private Material myBasicMaterial;

    // Start is called before the first frame update
    void Start()
    {
        myMesh = GetComponent<MeshRenderer>();

        myBasicMaterial = myMesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            myMesh.material = playerEnteredMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            myMesh.material = myBasicMaterial;
        }
    }
}
