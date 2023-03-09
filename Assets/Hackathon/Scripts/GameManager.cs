using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  
    public enum Scene
    {
        Island = 0,
        SpaceCalm = 1,
        SpaceFear = 2,
        UnderwaerCalm = 3,
        UnderwaterFear = 4
    }
    public enum Fear
    {
        Ocean,
        Space
    }
    [HideInInspector]
    public static GameManager Instance;
    [HideInInspector]
    public QuestionData quizQuestions;
    [HideInInspector]
    public Scene finalScene;
    [HideInInspector]
    public Fear fear;

  
    public GameObject player;
    public GameObject questionPanel;
    public Animator sceneTransition;
    public TextMeshProUGUI screenQuestion;
    public TextMeshProUGUI scoreInccorect;
    public TextMeshProUGUI scoreCorrect;
    public TextMeshProUGUI questionCount;
    public TextMeshProUGUI percentageToWin;
    private string _quizEndpoint;
    private int _correctAnswers = 0;
    private int _incorrectAnswers = 0;
    private int _totalQuestions;
    private int _answeredQuestions = 0;
    private bool _gameWon;
    private float _percentToWin;


    private void Awake()
    {
        Instance = this;
        _totalQuestions = Random.Range(5, 10);
        //Currently all categories are used, the difficulty is 'easy', and the total questions IS 20. All can be configured via UI in the future wanted to utilize the limted time for other aspects of the project
        _quizEndpoint = "https://the-trivia-api.com/api/questions?categories=food_and_drink,general_knowledge,geography,history,music,science,society_and_culture,sport_and_leisure,film_and_tv,arts_and_literature&limit=" + _totalQuestions + "& difficulty=easy";

    }
    // Start is called before the first frame update
    void Start()
    {
        LoadQuestions();
    }

    public void LoadQuestions()
    {

        var _request = WebRequest.Create(_quizEndpoint) as HttpWebRequest;

        _request.KeepAlive = true;
        _request.Method = "GET";
        _request.ContentType = "application/json; charset=utf-8";

        string _responseContent = null;

        try
        {
            using (var response = _request.GetResponse() as HttpWebResponse)
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    _responseContent = "{\"Questions\":" + reader.ReadToEnd() + "}";
                }
            }
            quizQuestions = JsonConvert.DeserializeObject<QuestionData>(_responseContent);
            _totalQuestions = quizQuestions.Questions.Count;
            questionCount.text = "0/" + _totalQuestions.ToString();
            _percentToWin = (float)(Random.Range(50, 90) * .01);

        }
        catch (WebException wex)
        {
            using (var Errreader = new StreamReader(wex.Response.GetResponseStream()))
            {
                Console.WriteLine(Errreader.ReadToEnd());
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void UpdateScore(bool _isCorrect)
    {
        if (_isCorrect){_correctAnswers++;}
        else{_incorrectAnswers++;}

        scoreCorrect.text = _correctAnswers.ToString();
        scoreInccorect.text = _incorrectAnswers.ToString();
        _answeredQuestions = _correctAnswers + _incorrectAnswers;
        questionCount.text = _answeredQuestions + "/" + _totalQuestions;
        percentageToWin.text = ((float)_correctAnswers / (float)_totalQuestions).ToString("##0%") + "/" + _percentToWin.ToString("##0%") + " (To Win)" ;
        _gameWon = ((float)_correctAnswers / (float)_totalQuestions) > _percentToWin;
        if (_answeredQuestions == _totalQuestions) { StartCoroutine(EndGame()); }
    }

    IEnumerator EndGame()
    {

        switch (fear)
        {
            case Fear.Ocean:
                finalScene = (_gameWon ? Scene.UnderwaerCalm : Scene.UnderwaterFear);
                break;
            case Fear.Space:
                finalScene = (_gameWon ? Scene.SpaceCalm : Scene.SpaceFear);
                break;
        }
        //yield to 'End game animation" method in the future
        yield return new WaitForSeconds(5.0f);
        //sceneTransition.SetTrigger("SceneFadeOut");
        sceneTransition.Play("SceneFadeOut", -1, 0.0f);
        SceneManager.UnloadScene((int)Scene.Island);
    }

 
}