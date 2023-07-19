//using Android.Widget;
using Microsoft.Maui.Controls.Shapes;
using GradientStop = Microsoft.Maui.Controls.GradientStop;
using LogicUnit;
using LogicUnit.Logic.GamePageLogic.Games.Snake;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
namespace UI.Pages;

public partial class GamePage : ContentPage
{

    private GameInformation m_GameInformation = GameInformation.Instance;
    private GameLibrary m_GameLibrary = new GameLibrary();
    private Game m_Game;
    private Dictionary<int,Image> m_GameImages = new Dictionary<int,Image>();
    private Dictionary<int,Button> m_gameButtons = new Dictionary<int, Button>();
    private int k = 0;

    public GamePage()
    {
        InitializeComponent();
        initializePage();
        m_Game.RunGame();
    }

    private void initializePage()
    {
        m_Game = m_GameLibrary.CreateAGame(eGames.Pacman);//m_GameInformation.m_NameOfGame);
        initializeEvents();
        initializeGame();

    }

    private void initializeGame()
    {
        m_Game.InitializeGame();
    }

    public void addGameObjects(object sender, List<GameObject> i_GameObjectsToAdd)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            int i = 1;
        foreach (var gameObject in i_GameObjectsToAdd)
        {
            if (gameObject.m_ScreenObjectType == eScreenObjectType.Button)
            {
                addButton(gameObject);
            }
            else// if (screenObject.m_ScreenObjectType == eScreenObjectType.Image)
            {
                addImage(gameObject);
            }
        }
        });
    }

    private void addImage(GameObject i_GameObjectToAdd)
    {
        Image image = new Image();

        image.TranslationX = i_GameObjectToAdd.m_PointsOnScreen[0].m_Column;
        image.TranslationY = i_GameObjectToAdd.m_PointsOnScreen[0].m_Row;
        image.Aspect = Aspect.AspectFill;

        if (i_GameObjectToAdd.m_OurSize.m_Width != 0)
        {
            image.WidthRequest = i_GameObjectToAdd.m_OurSize.m_Width;
        }

        if (i_GameObjectToAdd.m_OurSize.m_Height != 0)
        {
            image.HeightRequest = i_GameObjectToAdd.m_OurSize.m_Height;
            if (i_GameObjectToAdd.m_ImageSources[0] == "snakebackground.png")
            {
                
                image.Aspect = Aspect.AspectFill;
            }
            else
            {
                image.Aspect = Aspect.Fill;
            }
        }
        image.ClassId = i_GameObjectToAdd.m_ImageSources[0];
        image.Source = i_GameObjectToAdd.m_ImageSources[0];
        image.ZIndex = -1;
        image.Rotation = i_GameObjectToAdd.m_Rotatation[0];
        gridLayout.Add(image);
        m_GameImages.Add(i_GameObjectToAdd.m_ID[0], image);
    }


    private void addButton(GameObject i_ButtonToAdd)
    { 
        Button button = new Button();
        addImage(i_ButtonToAdd);
        button.ClassId = i_ButtonToAdd.m_ButtonType.ToString();//.m_KindOfButton.ToString();
        button.HeightRequest = i_ButtonToAdd.m_OurSize.m_Height;
        button.WidthRequest = i_ButtonToAdd.m_OurSize.m_Width;
        gridLayout.Add(button);
        button.TranslationX = i_ButtonToAdd.m_PointsOnScreen[0].m_Column;
        button.TranslationY = i_ButtonToAdd.m_PointsOnScreen[0].m_Row;
        
        //button.Text = i_ButtonToAdd.m_ButtonType.ToString();
        //button.Source = i_ButtonToAdd.m_ImageSources[0];
        button.Opacity = 0;
        //button.Aspect = Aspect.AspectFit;
        button.ZIndex = -1;
        m_gameButtons.Add(i_ButtonToAdd.m_ID[0],button);
        button.Clicked += m_Game.OnButtonClicked;
    }

    private void gameObjectsUpdate(object sender, List<GameObject> i_ObjectUpdates)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var screenObject in i_ObjectUpdates)
            {
                for(int i = 0; i < screenObject.m_ID.Count; i++)
                {
                    if(getObjectTypeFromID(screenObject.m_ID[i]) == eScreenObjectType.Image)
                    {
                        if(m_GameImages.ContainsKey(screenObject.m_ID[i]))
                        {
                            m_GameImages[screenObject.m_ID[i]].Rotation = 0;
                            m_GameImages[screenObject.m_ID[i]].ScaleX = 1;
                            m_GameImages[screenObject.m_ID[i]].ScaleY = 1;
                            m_GameImages[screenObject.m_ID[i]].Source = screenObject.m_ImageSources[i];
                            m_GameImages[screenObject.m_ID[i]].Rotation = screenObject.m_Rotatation[i];
                            m_GameImages[screenObject.m_ID[i]].ScaleX = screenObject.m_ScaleX[i];
                            m_GameImages[screenObject.m_ID[i]].ScaleY = screenObject.m_ScaleY[i];
                            m_GameImages[screenObject.m_ID[i]].TranslationX = screenObject.m_PointsOnScreen[i].m_Column;
                            m_GameImages[screenObject.m_ID[i]].TranslationY = screenObject.m_PointsOnScreen[i].m_Row;


                            // m_GameImages[screenObject.m_ID[i]].ScaleXTo(-1);
                            // m_GameImages[screenObject.m_ID[i]].ScaleX = -1;
                            //if (k % 2 == 0)
                            //{
                            //    m_GameImages[screenObject.m_ID[i]].ScaleX = 1;
                            //    // m_GameImages[screenObject.m_ID[i]].Rotation = 90;
                            //}

                            //m_GameImages[screenObject.m_ID[i]].TranslateTo(
                            //    screenObject.m_PointsOnScreen[i].m_Column,
                            //    screenObject.m_PointsOnScreen[i].m_Row);
                            //m_GameImages[screenObject.m_ID[i]].s

                        }
                    }
                }

                k++;
            }
        });
    }

    public void deleteObject(object sender, GameObject i_ObjectToDelete)
    {

        for (int i = 0; i < i_ObjectToDelete.m_ID.Count; i++)
        {
            if(i_ObjectToDelete.m_Fade)
            {
                m_GameImages[i_ObjectToDelete.m_ID[i]].FadeTo(0, 700, null);
            }
            else
            {
                m_GameImages[i_ObjectToDelete.m_ID[i]].FadeTo(0, 100, null);
            }
            //gridLayout.Remove(m_GameImages[i_ObjectToDelete.m_ID[i]]);
            //m_GameImages.Remove(i_ObjectToDelete.m_ID[i]);
        }
    }

    private void hideGameObjects(object sender, List<int> i_IDlist)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var ID in i_IDlist)
            {
                if(getObjectTypeFromID(ID) == eScreenObjectType.Image)
                {
                    m_GameImages[ID].IsVisible = false;
                }
                else
                {
                    m_gameButtons[ID].IsVisible = false;
                    m_gameButtons[ID].IsEnabled = false;
                }
            }

            if(m_Game.m_GameStatus != eGameStatus.Restarted || m_Game.m_GameStatus != eGameStatus.Ended)
            {
                //m_GameImages.Clear();
                //m_gameButtons.Clear();
                //startGame();
            }
        });
    }

    private void showGameObjects(object sender, List<int> i_IDlist)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var ID in i_IDlist)
            {
                if (getObjectTypeFromID(ID) == eScreenObjectType.Image)
                {
                    m_GameImages[ID].IsVisible = true;
                    m_GameImages[ID].ZIndex = 0;
                }
                else
                {
                    m_gameButtons[ID].IsVisible = true;
                    m_gameButtons[ID].IsEnabled = true;
                    m_gameButtons[ID].ZIndex = 1;
                }
            }
        });
    }

    private eScreenObjectType getObjectTypeFromID(int ID)
    {
        eScreenObjectType type = eScreenObjectType.Image;

        if(m_gameButtons.ContainsKey(ID))
        {
            type = eScreenObjectType.Button;
        }
        return type;
    }

    void initializeEvents()
    {
        m_Game.AddGameObjectList += addGameObjects;
        m_Game.GameObjectsUpdate += gameObjectsUpdate;
        m_Game.GameObjectToDelete += deleteObject;
        m_Game.GameObjectsToHide +=hideGameObjects;
        m_Game.GameObjectsToShow += showGameObjects;
    }
}

//playerObject.Source = screenObject.m_ImageSources[i];
//playerObject.TranslateTo(
//    screenObject.m_NewPositions[i].m_Column,
//    screenObject.m_NewPositions[i].m_Row);