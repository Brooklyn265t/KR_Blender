using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //Коэффициент чувствительности
    public float mouseSensitivity = 100f;

    //Сам объект FPS
    public Transform Controller;

    //Переменная, в которую будет записываться постоянно меняющиеся значение поворота
    float xRotation = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        //Блокировка курсора
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Ввод по оси x и оси y
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Огранничения угла обзора сверху и снизу, по 90 градусов каждое
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Поворот камеры по оси x
        Controller.Rotate(Vector3.up * mouseX);
    }
}

