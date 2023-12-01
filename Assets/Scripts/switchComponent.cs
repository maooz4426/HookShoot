using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchComponent : MonoBehaviour
{
    // Start is called before the first frame update

    private CharacterController characterController;
    private Rigidbody rigidbody;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
