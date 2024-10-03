//Sound 버튼 클릭 시 조절 창 활성화와 비활성화
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundExit : MonoBehaviour
{
    public GameObject SoundPanel;
    // Start is called before the first frame update
    // Update is called once per frame
 
    public void PanelOff()
    {
        SoundPanel.SetActive(false);
    }
    public void Panelon()
    {
        SoundPanel.SetActive(true);
    }
}
