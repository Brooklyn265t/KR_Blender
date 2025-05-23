using JetBrains.Annotations;
using UnityEngine;

public class WASD : MonoBehaviour{
    [SerializeField] private Transform cameraTransform;
    //Переменная со ссылкой на Character Controller
    public CharacterController CharacterController;

    //Параметр для изменения скорости передвижения
    public float speed = 12f;

    //Значение гравитационной постоянной(g) или просто гравитация
    public float gravity = -9.8f;

    //Переменная, в которой будет храниться значение при изменении скорости в падении
    Vector3 velocity;

    public Transform GroundCheck;

    //Расстояние до земли, на котором будет тригериться невидимая сфера
    public float groundDistance = 0.4f;

    //Маска для распохнования земли
    public LayerMask groundMask;

    //Переменная, которая изменит своё значение на true CharacterController на ground
    bool isGrounded;
    //ulong frame = 1;

    //Переменная сила прыжка
    public float jumpForce = 100;

    private float originalSpeed;
    private float currentSlowFactor = 1f;

    void Start(){
        originalSpeed = speed;
    }

    public void SetSlowFactor(float factor){
        currentSlowFactor = factor;
        speed = originalSpeed * currentSlowFactor;
    }

    // Update is called once p
    // er frame
    void Update(){
        //Debug.Log("Current position: " + transform.position);
        //Установка распознования ввода сигналов с клваиш WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //
        Vector3 move = transform.right * x + transform.forward * z;

        //Вызов метода Move() characterController с формулой для передвижения
        CharacterController.Move(move * speed * Time.deltaTime);

        //Изменение координаты Y в векторе velocity
        velocity.y += gravity + Time.deltaTime;

        //Ещё один вызов метода Move(), но уже для движения в падении
        //из логики формулы ускорения свободного падения h = (gt^2)/2
        CharacterController.Move((velocity * Time.deltaTime) / 2);

        //Переменная становиться истинной, если невидимая сфера, созданная в координатах GroundeCheck.possition
        //с радиусом groundDistance, соприкосается со слоем groundMask
        isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);
        //Debug.Log("Frame " + frame + ": isGrounded = " + isGrounded);
        //frame++;
        //Проверка истинности выражения
        //(Значение -2f берётся только из-за лучшего эффекта
        //значение 0f тоже допустимо)
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        //при нажатии клавиши Space по умолчанию, CharacterController подпрыгивает
        if (Input.GetButtonDown("Jump") && isGrounded) { 
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
        if(Input.GetKeyDown(KeyCode.E)){Interact();}
    }
    void Interact(){
        RaycastHit hit;
        if(cameraTransform == null){
            Debug.LogError("Allo gde camera");
            return;
        }
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3f)){
            Interactable other = hit.collider.GetComponent<Interactable>();
            if(other != null){
                other.Interact();
            }
        }

    }
}
