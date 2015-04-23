using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public GameObject tutorialScreen;
	public Button leftButton;
	public Button rightButton;
	public Image slide;
	public PlayerHub playerH;
	int currSlide = 0;
	int minSlide = 0;
	int maxSlide = 11;

	void Start(){
	 	slide.sprite = Resources.Load ("Tutorial/" + currSlide , typeof(Sprite)) as Sprite;
	}

	public void NextImage(bool str){
		if (str) {
			currSlide++;
			slide.sprite = Resources.Load ("Tutorial/" + currSlide , typeof(Sprite)) as Sprite;
			leftButton.interactable = true;
			if(currSlide >= maxSlide)
				rightButton.interactable = false;
		}
	}
	public void PrevImage(bool str){
		if (str) {
			currSlide--;
			slide.sprite = Resources.Load ("Tutorial/" + currSlide , typeof(Sprite)) as Sprite;
			rightButton.interactable = true;
			if(currSlide <= minSlide)
				leftButton.interactable = false;
		}
	}
	public void CloseTutorial(bool str){
		if (str) {
			playerH.cinematicMode = false;
			playerH.wantedCursorLock = CursorLockMode.Locked;
		}
	}



}
