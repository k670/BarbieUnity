using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WorkScript : MonoBehaviour
{
   
   public Text timeText;
   public GameObject resultPanel;
   public Text resultText;
   

	public float timeLeft;
	public string userServerHost;
	public int userId;
	
	private float timer;
	
	private bool isStarted = false;
	
	private int countOfClick;
	
	void Start(){
		timer = timeLeft;
	}
	
	
	public void startWork(){
		isStarted = true;
		countOfClick = 0;
	}

    // Update is called once per frame
    void Update()
    {
		if(isStarted){
			
			timeText.text = ""+timer;
			 timer -= Time.deltaTime;
			 
			 if(timer < 0)
			 {
				 timerFinished();
			 }
		}
    }
	
	public void coinsGenerate(){
		countOfClick++;
		Debug.Log(countOfClick);
	}
	
	private void timerFinished(){
		int salary = 10*countOfClick;
		AccountBalanceDiffDTO dto = new AccountBalanceDiffDTO(userId,salary);
		string json =  JsonUtility.ToJson(dto);
		StartCoroutine(PostRequest(userServerHost+"account", json));
		
		timeText.text = "0";
		isStarted = false;
		Debug.Log("end");
		resultPanel.SetActive(true);
		resultText.text = "Your salary: "+salary;
		timer = timeLeft;
	}
	
	 
    
	 IEnumerator PostRequest(string url, string json)
	 {
		 var uwr = new UnityWebRequest(url, "POST");
		 byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
		 uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
		 uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		 uwr.SetRequestHeader("Content-Type", "application/json");

		 //Send the request then wait here until it returns
		 yield return uwr.SendWebRequest();

		 if (uwr.isNetworkError)
		 {
			 Debug.Log("Error While Sending: " + uwr.error);
		 }
		 else
		 {
			 Debug.Log("Received: " + uwr.downloadHandler.text);
		 }
	 }
}

[System.Serializable]
public class AccountBalanceDiffDTO {
    public int accountId;
    public int balanceDifference;
	
	public AccountBalanceDiffDTO(int accountId, int balanceDifference){
		this.accountId = accountId;
		this.balanceDifference = balanceDifference;
	}
}