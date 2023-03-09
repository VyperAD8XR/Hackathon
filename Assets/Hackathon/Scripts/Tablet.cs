using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using Unity.VisualScripting;

public class Tablet : MonoBehaviour
{

    const string _colorCorrect = "1F6305";
    const string _colorIncorrect = "4B0A0B";

    [HideInInspector]
    public static Tablet Instance;
    public Button answerButtonTemplate;
    public Button nextQuestionButton;

    private string _correctAnswer;
    private int _correctAnswerIndex;
    private List<Button> _answerButtons = new();
    private bool _answerSelected;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {


        LoadQuestion();
    }


   
    public void LoadQuestion()
    {
        QuestionsObject _question;
        int _answerIndex;
        int _questaionIndex;
        List<Button> _availableButtons = new();
        string _answerButtonText;

        nextQuestionButton.gameObject.SetActive(false);
        GameManager.Instance.questionPanel.GetComponent<Image>().color = Color.gray;
        _answerSelected = false;
        _answerButtons.Clear();
        _questaionIndex = Random.Range(0, GameManager.Instance.quizQuestions.Questions.Count - 1);
        _question = GameManager.Instance.quizQuestions.Questions[_questaionIndex];
       
        GameManager.Instance.screenQuestion.text = _question.question;


        foreach (GameObject _answerButton in GameObject.FindGameObjectsWithTag("AnswerButton"))
        {
            GameObject.Destroy(_answerButton);
        }
        answerButtonTemplate.gameObject.SetActive(true);

        for (int i = 0; i < _question.incorrectAnswers.Count + 1; i++)
        {

            _answerButtons.Add(Instantiate(answerButtonTemplate, answerButtonTemplate.transform.parent.transform));
            _answerButtons[i].tag = "AnswerButton";
        }

        _availableButtons.AddRange(_answerButtons);
        _correctAnswer = _question.correctAnswer;

        while (_availableButtons.Count != 0)
        {
            _answerIndex = Random.Range(0, _availableButtons.Count);
            if (_availableButtons.Count == _question.incorrectAnswers.Count + 1)
            {
                _answerButtonText = _correctAnswer;
                _availableButtons[_answerIndex].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _correctAnswer;
                _correctAnswerIndex = _answerIndex;
            }
            else
            {
                _answerButtonText = _question.incorrectAnswers[0];
                _question.incorrectAnswers.RemoveAt(0);
            }
            _availableButtons[_answerIndex].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _answerButtonText;
            _availableButtons.RemoveAt(_answerIndex);
        }
        GameManager.Instance.quizQuestions.Questions.RemoveAt(_questaionIndex);
        answerButtonTemplate.gameObject.SetActive(false);

    }

    public void ButtonClick(Button _button)
    {
        switch (_button.tag)
        {
            case "AnswerButton":

                if (_answerSelected) { return; }
                GameManager.Instance.UpdateScore(CheckAnswer(_button));

                break;
            case "NextQButton":
                LoadQuestion();
                break;
        }
    }

    bool CheckAnswer(Button _button)
    {

        TextMeshProUGUI _buttonTextComponent;
        bool _isCorrect;

        _buttonTextComponent = _button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        _isCorrect = _buttonTextComponent.text == _correctAnswer;
        _buttonTextComponent.color = (_isCorrect ? Color.black : Color.white);
        _button.GetComponent<Button>().enabled =false;
        _button.GetComponent<Image>().color = (_isCorrect ? Color.green : Color.red);
        GameManager.Instance.questionPanel.GetComponent<Image>().color = (_isCorrect ? Color.green : Color.red);

        if (!_isCorrect)
        {
            _answerButtons[_correctAnswerIndex].GetComponent<Image>().color = Color.green;
        }
        _answerSelected = true;
        nextQuestionButton.gameObject.SetActive(true);
        return _isCorrect;
    }


}
