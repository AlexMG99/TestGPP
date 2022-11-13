using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;
    const string display = "{0} FPS \n {1} ms";
    private TMPro.TextMeshProUGUI m_Text;


    private void Start()
    {
        m_Text = GetComponent<TMPro.TextMeshProUGUI>();
    }


    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;

            m_Text.text = string.Format(display, (int)m_lastFramerate, (int)(Time.deltaTime * 1000));
        }
    }

}
