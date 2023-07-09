using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    public Slider slider;
    public Image faceImage;
    
    public List<Sprite> faceList = new List<Sprite>();
    public float rotateSpeed;
    public float shakeIntensity;
    
    private int imagePicker;
    private float sliderMin;
    private float sliderMax;
    private float zRotation;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    void Start()
    {
        sliderMin = slider.minValue;
        sliderMax = slider.maxValue;
        rotateSpeed = 100;
        shakeIntensity = 5;
    }

    public void SetSliderValue(float setVal)
    // void Update()
    {
        // float setVal = slider.value;
        slider.value = setVal;
        int tmp = Mathf.FloorToInt(Remap(setVal,sliderMin,sliderMax,0,(float)faceList.Count-1));
        faceImage.sprite = faceList[tmp];
        transform.rotation = Quaternion.Euler(0, 0, (Mathf.Sin(Time.time*rotateSpeed))*(shakeIntensity*(setVal))); //new Quaternion(0, 0, (Mathf.Sin(Time.time)*10)*sValue);
    }
    
    public static float Remap ( float from, float fromMin, float fromMax, float toMin,  float toMax)
    {
        var fromAbs  =  from - fromMin;
        var fromMaxAbs = fromMax - fromMin;      
       
        var normal = fromAbs / fromMaxAbs;
 
        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;
 
        var to = toAbs + toMin;
       
        return to;
    }

    private void ShakeBar()
    {
        
    }
}
