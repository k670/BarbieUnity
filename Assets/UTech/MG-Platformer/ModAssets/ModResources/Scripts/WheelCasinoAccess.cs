using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WheelCasinoAccess : MonoBehaviour
{
	public int userId;
	public string serverHost;
	
	public GameObject InfoPanel;
	public Text InfoText;
	
	public Button SpinButton;
	
    public void spinAccess(){
		
		StartCoroutine(GetText("/wheelfortune/"+userId));
    }
 
    IEnumerator GetText(string path) {
        UnityWebRequest www = UnityWebRequest.Get(serverHost+path);
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
			string result = www.downloadHandler.text;
            Debug.Log(result);
			WheelFortuneStatusModel wheelfortuneStatus = JsonUtility.FromJson<WheelFortuneStatusModel>(result);
			
			if(wheelfortuneStatus.timeToWait==0&&wheelfortuneStatus.enoughMoney){
				SpinButton.enabled=true;
			}else{
					
				InfoPanel.SetActive(true);
				if(wheelfortuneStatus.timeToWait>0){
					InfoText.text = "Time to wait: "+FormatTime(wheelfortuneStatus.timeToWait);
				}else{
					if(wheelfortuneStatus.enoughMoney==false){					
						InfoText.text = "You dont have enough money";
					}else{					
						InfoText.text = "Oops...\nServer error";
					}
				}
			}
        }
    }	
	string FormatTime (long time){
         int intTime = (int)time/1000;
         int seconds = intTime % 60;
		 intTime = intTime/60;
         int minutes = intTime % 60;
		 intTime = intTime/60;
		 int hours = intTime % 60;
		 
         string timeText = System.String.Format ("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
         return timeText;
     }
}

	 

[System.Serializable]
public class WheelFortuneStatusModel {
    public long price;
    public long timeToWait;
	public bool enoughMoney;
}