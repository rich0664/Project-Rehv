using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InkManager : MonoBehaviour {

	float wallet;
	float redInk;
	float greenInk;
	float blueInk;
	float whiteInk;
	float rubberInk;

	public GameObject moneyWarning;
	public Text walletText;
	public Slider redSlider;
	public Slider greenSlider;
	public Slider blueSlider;
	public Slider whiteSlider;
	public Slider rubberSlider;

	public GameObject lowInkR;
	public GameObject lowInkG;
	public GameObject lowInkB;
	public GameObject lowInkW;
	public GameObject lowInkRubber;

	public Button refillR;
	public Button refillG;
	public Button refillB;
	public Button refillW;
	public Button refillRubber;

	public Text refillTextR;
	public Text refillTextG;
	public Text refillTextB;
	public Text refillTextW;
	public Text refillTextRubber;


	// Use this for initialization
	void Start () {
		LoadCartValues ();
		LoadWallet ();
	}

	IEnumerator flashWarning(float delay, int flashes){

		for (int i = 0; i < flashes; i++) {
			moneyWarning.SetActive (true);
			yield return new WaitForSeconds (delay);
			moneyWarning.SetActive (false);
			yield return new WaitForSeconds (delay);
		}
		moneyWarning.SetActive (true);
		yield return new WaitForSeconds (delay + 1.5f);
		moneyWarning.SetActive (false);

	}


	public void buyCart(string cart){
		float rubberPrice = 35f;
		float inkPrice = 20f;

		if (wallet > inkPrice && cart != "Rubber") {
			SaveLoad.SaveFloat (cart + "Ink", 1f);
			wallet -= inkPrice;
		} else {
			if(wallet > rubberPrice){
				SaveLoad.SaveFloat (cart + "Ink", 1f);
				wallet -= rubberPrice;
			} else {
				StartCoroutine(flashWarning(0.20f, 3));
			}
		}

		SaveLoad.SaveFloat ("Money", wallet);
		LoadCartValues ();
		LoadWallet ();
	}

	public void modifiedInks(string str){
		str = str;

		redInk = redSlider.value;
		greenInk = greenSlider.value;
		blueInk = blueSlider.value;
		whiteInk = whiteSlider.value;
		rubberInk = rubberSlider.value;

		SaveLoad.SaveFloat ("RedInk", redInk);
		SaveLoad.SaveFloat ("GreenInk", greenInk);
		SaveLoad.SaveFloat ("BlueInk", blueInk);
		SaveLoad.SaveFloat ("WhiteInk", whiteInk);
		SaveLoad.SaveFloat ("RubberInk", rubberInk);

		LoadCartValues ();

	}

	void LoadCartValues(){

		float lowInkThreshold = 0.25f;
		string refillString = "Refill";

		redInk = SaveLoad.LoadFloat("RedInk");
		greenInk = SaveLoad.LoadFloat("GreenInk");
		blueInk = SaveLoad.LoadFloat("BlueInk");
		whiteInk = SaveLoad.LoadFloat("WhiteInk");
		rubberInk = SaveLoad.LoadFloat("RubberInk");
		
		redSlider.value = redInk;
		greenSlider.value = greenInk;
		blueSlider.value = blueInk;
		whiteSlider.value = whiteInk;
		rubberSlider.value = rubberInk;

		//LOW INK-------------------------------------------------------------------------

		if(redInk < lowInkThreshold){ 
			lowInkR.SetActive(true);
		} else { lowInkR.SetActive(false); }

		if(greenInk < lowInkThreshold){ 
			lowInkG.SetActive(true);
		} else { lowInkG.SetActive(false); }

		if(blueInk < lowInkThreshold){ 
			lowInkB.SetActive(true);
		} else { lowInkB.SetActive(false); }

		if(whiteInk < lowInkThreshold){ 
			lowInkW.SetActive(true);
		} else { lowInkW.SetActive(false); }

		if(rubberInk < lowInkThreshold){ 
			lowInkRubber.SetActive(true);
		} else { lowInkRubber.SetActive(false); }

		//FULL------------------------------------------------------------
		if(redInk >= 1f){ 
			refillR.interactable = false;
			refillTextR.text = "Full";
		} else { refillR.interactable = true; refillTextR.text = refillString;}
		
		if(greenInk >= 1f){ 
			refillG.interactable = false;
			refillTextG.text = "Full";
		} else { refillG.interactable = true; refillTextG.text = refillString;}
		
		if(blueInk >= 1f){ 
			refillB.interactable = false;
			refillTextB.text = "Full";
		} else { refillB.interactable = true; refillTextB.text = refillString;}
		
		if(whiteInk >= 1f){ 
			refillW.interactable = false;
			refillTextW.text = "Full";
		} else { refillW.interactable = true; refillTextW.text = refillString;}
		
		if(rubberInk >= 1f){ 
			refillRubber.interactable = false;
			refillTextRubber.text = "Full";
		} else { refillRubber.interactable = true; refillTextRubber.text = refillString;}

	}

	void LoadWallet(){
		wallet = SaveLoad.LoadFloat("Money");
		walletText.text = "Wallet: $" + wallet.ToString("F2");
	}



//END OF CLASS-------------------------------------------------------------------------------------
}
