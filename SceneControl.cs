//게임화면으로 이동하거나 Exit버튼을 눌렀을 때 실행되는 코드

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneControl : MonoBehaviour
{

    // Start is called before the first frame update
    public void GoToMainScene()
    {
        SceneManager.LoadScene(1);//씬 이동
    }
    public void ExitGame()
    {
        Application.Quit();//게임 종료
    }
  
}
