using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossColor : MonoBehaviour
{
    public MeshRenderer Renderer;
    public float colorChangeSpeed;
    //public float beatSpeed;

    [SerializeField] private float[] colors;
    private int colorIndex;
    private int colorDirection;
    private bool allowColorChange = true;

    //[SerializeField] private float currentBeat;

    void Start()
    {
        InitializeColor();
        //currentBeat = 0;
        //transform.position = new Vector3(3, 4, 1);
    }

    void Update()
    {
        //transform.Rotate(10.0f * Time.deltaTime, 0.0f, 0.0f);
        //ChangeBeat();
        if (allowColorChange)
        {
            ChangeColor();
        }
        else
        {
            GoToBlack();
        }
    }

    private void InitializeColor()
    {
        colors = new float[] { 1, 0, 0 };
        if (colorChangeSpeed < 0) { colorChangeSpeed = 0; }
        colorIndex = 2;
        colorDirection = 1;
    }

    private void ChangeColor()
    {
        Renderer.material.color = GetColor();
        //new Color(0.5f, 1.0f, 0.3f, 0.4f);
    }

    private Color GetColor()
    {
        // calcular la suma de color
        // agregar la suma
        // si la suma es >= 1 o <=0, limitar, cambiar dirección, y cambiar de color

        float colorChange = colorChangeSpeed * Time.deltaTime * colorDirection;
        colors[colorIndex] += colorChange;
        if (colors[colorIndex] < 0)
        {
            colors[colorIndex] = 0;
        }
        else if (colors[colorIndex] > 1)
        {
            colors[colorIndex] = 1;
        }

        if (colors[colorIndex] == 0 || colors[colorIndex] == 1)
        {
            colorIndex++;
            colorDirection *= -1;
        }

        if (colorIndex > 2)
        {
            colorIndex = 0;
        }

        return new Color(colors[0], colors[1], colors[2], 1);
    }

    private void GoToBlack()
    {
        float colorChange = colorChangeSpeed * Time.deltaTime;
        colors[0] -= colorChange;
        colors[1] -= colorChange;
        colors[2] -= colorChange;
        if (colors[0] <= 0) colors[0] = 0;
        if (colors[1] <= 0) colors[1] = 0;
        if (colors[2] <= 0) colors[2] = 0;
        Renderer.material.color = new Color(colors[0], colors[1], colors[2], 1);
    }

    public void TurnOffColor()
    {
        allowColorChange = false;
    }

    /*
    private void ChangeBeat()
    {
        float size;
        currentBeat += beatSpeed * Time.deltaTime;
        if (currentBeat >= 2)
        {
            currentBeat = 0;
        }

        size = Mathf.Pow(Mathf.Sin(currentBeat), 63) * Mathf.Sin(currentBeat + 1.5f) * 8;
        transform.localScale = Vector3.one * (size * 1.5f + 1);
    }
    */
}
