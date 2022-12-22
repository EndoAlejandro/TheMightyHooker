using PlayerComponents;
using UI;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private UITutorialMenu tutorialUI;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Player player))
            tutorialUI.DisplayTutorial();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
            tutorialUI.TurnOffDisplay();
    }

    public void Setup(UITutorialMenu uiTutorialMenu) => tutorialUI = uiTutorialMenu;
}