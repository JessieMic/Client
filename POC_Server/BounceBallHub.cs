using Microsoft.AspNetCore.SignalR;


public class BounceBallHub : Hub
{
    static double s_XDirectionBounceFactor = 0;
    static double s_YDirectionBounceFactor = 0;
    //static PeriodicTimer periodicTimer;

    public override Task OnConnectedAsync()
    {
        ////Clients.All.SendAsync("UpdateScreenSize");
        ////Maybe here we wont use all 
        return base.OnConnectedAsync();
    }

    //public async Task saveDimentions(string numberOfSpot, string width, string height/*, string id*/)
    //{
    //    int spot;

    //    if (int.TryParse(numberOfSpot, out spot) && savedDimentions<4)
    //    {
    //        spot--;
    //        dimentions[spot] = new Dimentions { Width = Double.Parse(width), Height = Double.Parse(height) };
    //        savedDimentions++;
    //        //connectionIDs[spot] = id;
    //        connectionIDs[spot] = Context.ConnectionId;
    //        if (savedDimentions == 4)
    //        {
    //            dimentions[0].Height = dimentions[1].Height = dimentions[2].Height = dimentions[3].Height = 263;
    //            dimentions[0].Width = dimentions[1].Width = dimentions[2].Width = dimentions[3].Width = 523;

    //            totalWidth = double.Min(dimentions[0].Width, dimentions[2].Width) +
    //                double.Min(dimentions[1].Width, dimentions[3].Width);

    //            totalHeight = double.Min(dimentions[0].Height, dimentions[1].Height) +
    //                double.Min(dimentions[2].Height, dimentions[3].Height);

    //            /*  -------
    //             *   0 | 1
    //             *  -------
    //             *   2 | 3
    //             *  -------
    //             */

    //            //await Clients.All.SendAsync("UpdateBoardDimentions", totalWidth, totalHeight);

    //            //await Clients.Client(connectionIDs[0]).SendAsync("UpdateBoardDimentions",
    //            //    totalWidth, totalHeight, 0, 0); // # 1
    //            //await Clients.Client(connectionIDs[1]).SendAsync("UpdateBoardDimentions",
    //            //    totalWidth, totalHeight, -dimentions[0].Width, 0); // # 2
    //            //await Clients.Client(connectionIDs[2]).SendAsync("UpdateBoardDimentions",
    //            //    totalWidth, totalHeight, 0, -dimentions[0].Height); // # 3
    //            //await Clients.Client(connectionIDs[3]).SendAsync("UpdateBoardDimentions",
    //            //    totalWidth, totalHeight, -dimentions[0].Width, -dimentions[0].Height); // # 4
    //        }
    //    }
    ////}
    //periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(5));

    //    while (await periodicTimer.WaitForNextTickAsync())//Keep bouncing ball untill this flag is 'true'
    //    {

    //public Task StopBouncingBall()
    //{
    //    ballBouncing = false;
    //    periodicTimer.Dispose();

    //    return Task.CompletedTask;
    //}

    //public async Task UpdateGame(string totalWidth_, string totalHeight_, string mainWidth_, string mainHeight_
    //    ,string h0,string h1,string h2, string w0, string w1,string w2)
    //{
    //    double totalWidth = double.Parse(totalWidth_);
    //    double totalHeight = double.Parse(totalHeight_);

    //    ballXPosition += xDirectionBounceFactor;
    //    increment or decrement y direction of the ball
    //    ballYPosition += yDirectionBounceFactor;

    //    Translates the ball to next x and y cordinates
    //    await ball.TranslateTo(ballXPosition, ballYPosition, 1);

    //    if ball's 'x' cordinate reaches end of the screen width
    //    if (ballXPosition >= totalWidth)// - 40)
    //    {
    //        xDirectionBounceFactor = -7;
    //    }
    //    if ball's 'x' cordinate reaches start of the screen width
    //    if (ballXPosition <= 0)
    //    {
    //        xDirectionBounceFactor = 7;
    //    }
    //    if ball's 'y' cordinate reaches end of the screen height
    //    if (ballYPosition >= (totalHeight))// - 100)
    //    {
    //        yDirectionBounceFactor = -5;
    //    }
    //    if ball's 'x' cordinate reaches start of the screen height
    //    if (ballYPosition <= 0) //if (ballYPosition <= -(screenHeight / 2)+75)
    //    {
    //        yDirectionBounceFactor = 5;
    //    }
    //    await Clients.All.SendAsync("MoveBall", ballXPosition, ballYPosition,h0, h1, h2, w0, w1, w2);
    //    await Clients.All.SendAsync("MoveBall", ballXPosition, ballYPosition, mainWidth_, mainHeight_,h0,h1,h2,w0,w1,w2);
    //}

    //public async Task UpdateGame(string totalWidth, string totalHeight, string ballXPosition, string ballYPosition)
    //{
    //    double totalWidth = double.Parse(totalWidth_);
    //    double totalHeight = double.Parse(totalHeight_);
    //    double ballXPosition = double.Parse(ballXPosition_);
    //    double ballYPosition = double.Parse(ballYPosition_);

    //    ballXPosition += s_XDirectionBounceFactor;
    //    //increment or decrement y direction of the ball
    //    ballYPosition += s_YDirectionBounceFactor;

    //    //Translates the ball to next x and y cordinates
    //    //await ball.TranslateTo(ballXPosition, ballYPosition, 1);

    //    //if ball's 'x' cordinate reaches end of the screen width
    //    if (ballXPosition >= totalWidth - 45)
    //    {
    //        s_XDirectionBounceFactor = -7;
    //    }
    //    //if ball's 'x' cordinate reaches start of the screen width
    //    if (ballXPosition <= 0)
    //    {
    //        s_XDirectionBounceFactor = 7;
    //    }
    //    //if ball's 'y' cordinate reaches end of the screen height
    //    if (ballYPosition >= totalHeight - 90)
    //    {
    //        s_YDirectionBounceFactor = -7;
    //    }
    //    //if ball's 'x' cordinate reaches start of the screen height
    //    if (ballYPosition <= 0) //if (ballYPosition <= -(screenHeight / 2)+75)
    //    {
    //        s_YDirectionBounceFactor = 7;
    //    }

    //    await Clients.All.SendAsync("MoveBall", ballXPosition, ballYPosition);

    //}

    public async Task StartBouncingBall()
    {
        await Clients.All.SendAsync("StartBouncingBall");
    }

    public async Task StopBouncingBall()
    {
        await Clients.All.SendAsync("StopBouncingBall");
    }

    public async Task SendClientsScreenValues(string h0, string h1, string h2, string h3, string w0,
        string w1, string w2, string w3, string totalWidth, string totalHeight)
    {
        await Clients.All.SendAsync("UpdateScreenValues", totalWidth, totalHeight);

        await Clients.All.SendAsync("GetScreenValues", h0, h1, h2, h3, w0, w1, w2, w3);
    }

    public async Task UpdateBallPosition(string ballXPosition, string ballYPosition)
    {
        double ballX = double.Parse(ballXPosition);
        double ballY = double.Parse(ballYPosition);

        await Clients.All.SendAsync("GetupdateBallPosition",ballX,ballY);
    }
}

