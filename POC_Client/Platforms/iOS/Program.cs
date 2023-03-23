using ObjCRuntime;
using UIKit;

namespace POC_Client;

public class Program
{
	// This is the main entry point of the application.
	static void main(string[] args)
	{
		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}
