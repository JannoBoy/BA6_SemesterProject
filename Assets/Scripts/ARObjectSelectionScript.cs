using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARObjectSelectionScript : MonoBehaviour
{

    [SerializeField]
    private MeshFilter myMesh;

    [SerializeField]
    private Mesh[] selectableMesh = new Mesh[2];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlaceARObject()
    {
        GameObject newObject = GameObject.Instantiate(gameObject);
    }

    public void SelectCube()
    {
        myMesh.mesh = selectableMesh[0];
    }

    public void SelectCylinder()
    {
        myMesh.mesh = selectableMesh[1];
    }

    public void SelectCapsule()
    {
        myMesh.mesh = selectableMesh[2];
    }
}
