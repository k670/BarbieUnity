using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using UnityEditor;

public class WheelCasinoClothesGenerator : MonoBehaviour
{
	
	public Animation Anim;
	
	public string serverHost;
	
	public string casinoGameName;
	
	public Image ClothesImage;
	
	public GameObject WinPanel;
	
	public int userId;
	
	public Sprite[] clothesStore;
	

    // Update is called once per frame
    void Update()
    {
       //   if (Input.GetMouseButtonDown(0))            Debug.Log("Pressed left click.");
		
    }
	
	public void runAnimation()
	{
		Anim.enabled=true;
		Anim.Play();
	    StartCoroutine(GetText("/wheelfortune/spin/"+userId));
    }
 
    IEnumerator GetText(string path) {
        UnityWebRequest www = UnityWebRequest.Get(serverHost+path);
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
			stopAnimation();
        }
        else {
            // Show results as text
			string result = www.downloadHandler.text;
            Debug.Log(result);
			UserWinModel userWinModel = JsonUtility.FromJson<UserWinModel>(result);
			stopAnimation();
 
			if(casinoGameName=="wheel"){
				afterGetWheelResult(userWinModel.clothesName);
			}
        }
    }	
	
	void afterGetWheelResult(string data){
		
		WinPanel.SetActive(true);
		Sprite clothes=null;
		for(int i=0;i<clothesStore.Length;i++){
			if(clothesStore[i].name==data){
				clothes = clothesStore[i];
			}
		}
		if(clothes!=null){			
			ClothesImage.sprite =  clothes;
		}
	}
	
	
	public void stopAnimation()
	{
		Anim.enabled=false;
	}	

}

[System.Serializable]
public class UserWinModel {
    public string clothesName;
}
