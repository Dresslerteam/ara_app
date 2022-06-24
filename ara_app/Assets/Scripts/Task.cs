using System;

namespace ARA.Frontend
{
    [Serializable]
    public class Task
    {
        public string taskTitle;
        public bool isComplete = false;
        
        // If task has a video link, it should enable a videoPlayer Ui
        
        // Todo: The windows that pop up for the menu, should re-order themselves. Think like a grid? 
        // Todo: work on the quick menu thing
    }
}