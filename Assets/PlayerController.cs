using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform rifleStart;
    [SerializeField] private Text HpText;

    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject Victory;

    private CharacterController _controller;
    float _moveSpeed = 5f;
    float _gravity = -9.81f;
    [SerializeField] float _jumpHeight = 2f;
    [SerializeField] private Transform _playerCameraTransform;
    private Vector3 _velocity;
   

    public float health = 0;

    void Start()
    {
        _controller = GetComponent<CharacterController>();   
    }

    public void ChangeHealth(int hp)
    {
        health += hp;
        if (health > 100)
        {
            health = 100;
        }
        else if (health <= 0)
        {
            Lost();
        }
        HpText.text = health.ToString();
    }

    public void Win()
    {
        Victory.SetActive(true);
        Destroy(GetComponent<PlayerLook>());
        Cursor.lockState = CursorLockMode.None;
    }

    public void Lost()
    {
        GameOver.SetActive(true);
        Destroy(GetComponent<PlayerLook>());
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {   

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = _playerCameraTransform.right * horizontalInput + _playerCameraTransform.forward * verticalInput;
        moveDirection.y = 0f; 

        _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);

        if (_controller.isGrounded)
        {
            _velocity.y = 0f;

            if (Input.GetButtonDown("Jump"))
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity); 
            }
        }

        _velocity.y += _gravity * Time.deltaTime; 
        _controller.Move(_velocity * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject buf = Instantiate(bullet);
            buf.transform.position = rifleStart.position;
            buf.GetComponent<Bullet>().setDirection(transform.forward);
            buf.transform.rotation = transform.rotation;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            Collider[] tar = Physics.OverlapSphere(transform.position, 2);
            foreach (var item in tar)
            {
                if (item.tag == "Enemy")
                {
                    Destroy(item.gameObject);
                }
            }
        }

        Collider[] targets = Physics.OverlapSphere(transform.position, 3);
        foreach (var item in targets)
        {
            if (item.tag == "Heal")
            {
                ChangeHealth(50);
                Destroy(item.gameObject);
            }
            if (item.tag == "Finish")
            {
                Win();
            }
            if (item.tag == "Enemy")
            {
                Lost();
            }
        }
    }
}
