using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DataModels : MonoBehaviour
{
  
}
//[Serializable]
public class QuestionsObject
{
    public string category;
    public string id;
    public string correctAnswer;
    public List<string> incorrectAnswers;
    public string question;
    public List<string> tags;
    public string type;
    public string difficulty;
    public List<string> regions;
    public bool isNiche;
}
//[Serializable]
public class QuestionData
{
    public List<QuestionsObject> Questions;
}