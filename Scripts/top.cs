using Godot;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

public partial class top : Control
{
	//cmdLine CL = new cmdLine();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//load settings
		settingsLocation = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), settingsLocation);
		GD.Print(settingsLocation);
		if (!System.IO.File.Exists(settingsLocation))
		{
			GD.Print("File doesn't exist");
			settingsCurrent = false;
		}
		else
		{
			 string s =  File.ReadAllText (settingsLocation);
			if(Directory.Exists(s)) { sharkLocation = s;}
		}
        outputLocation = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Mori-Shark-Factory\\");
        if (!Directory.Exists(outputLocation))
		{
			Directory.CreateDirectory(outputLocation);
		}
		opt = GetNode<OptionButton>("OptionButton");
		tex = GetNode<TextEdit>("Data Entry/text");
	}
	string settingsLocation = "mori-SHARK-factory\\settings.txt";
	string settingsDirectory = "mori-SHARK-factory";
	OptionButton opt;
	TextEdit tex;
	string sharkLocation;
	string outputLocation;
	bool settingsCurrent;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void _on_text_edit_text_changed()
	{
		GD.Print("changed");
		if(opt.Text == "Prompt")
		{
			prompt = tex.Text;
			GD.Print(opt.Text + " text is now: " + prompt);
		}
		if(opt.Text == "Negative Prompt")
		{
			negativePrompt = tex.Text;
		}
		if(opt.Text == "Seed")
		{

			int c = seed;
			int.TryParse(tex.Text, out c);
			seed = c;
		}
		if(opt.Text == "Shark Install")
		{
			sharkLocation = tex.Text;
			settingsCurrent = false;
		}
		if(opt.Text == "Output Directory")
		{
			outputLocation = tex.Text;
		}

	}
	public void doop(int blah)
	{
		GD.Print("Something or other");
	}
	public void _onOptionChange(int blah)
	{
		if (opt.Text == "Prompt") { 
			GetNode<Control>("Prompts").Visible = true;
			GetNode<Control>("Data Entry").Visible = false;
		} else { GetNode<Control>("Prompts").Visible = false; GetNode<Control>("Data Entry").Visible = true; }
		if(opt.Text == "Negative Prompt") { tex.Text = negativePrompt; }
		if(opt.Text == "Seed") { tex.Text = seed.ToString(); }
		if (opt.Text == "Shark Install") { tex.Text = sharkLocation; }
		if(opt.Text == "Output Directory") { tex.Text = outputLocation; }
	}
	
	public override void _Process(double delta)
	{
		if(werking == true)
		{
			if (ta.IsCompleted)
			{
				werking = false;
                GetNode<Button>("Button").Text = "Generate";
            }
		}
	}
	string prompt = "";
	string negativePrompt = "";
	int seed = 42;
	Task ta;
	bool werking = false;
	public void generate()
	{
		if (System.IO.Directory.Exists(outputLocation + "\\sharkOutputs") == false)
		{
			System.IO.Directory.CreateDirectory(outputLocation + "\\sharkOutputs");
		}
		if(settingsCurrent == false)
		{
			settingsCurrent = true;
			GD.Print(settingsLocation);
			GD.Print(sharkLocation);
			string p = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),settingsDirectory);
			GD.Print(p);
			if(!Directory.Exists(p)) { System.IO.Directory.CreateDirectory(p); }
			System.IO.File.WriteAllText(settingsLocation, sharkLocation);
		}
		if (GetNode<Button>("Button").Text == "Generate")
		{
			GetNode<Button>("Button").Text = "WORKING";
            prompt = GetNode<TextEdit>("Prompts/Positive").Text.Trim();
            negativePrompt = GetNode<TextEdit>("Prompts/Negative").Text.Trim();
            string se = GetNode<LineEdit>("Prompts/Seed").Text;
			int runs = 1;
			string rs = GetNode<LineEdit>("Prompts/Runs").Text;

			int.TryParse(rs, out runs);
			if(runs < 1) { runs = 1; }
			string ss = GetNode<LineEdit>("Prompts/Steps").Text;
			int steps = 0;
			int.TryParse(ss, out steps);
			if(steps <1) { steps = 1; }
			

            ta = Task.Run(() =>
			{
				werking = true;
				Process cmd;

				//generating the 
				int seed = 0;
				se = GetNode<LineEdit>("Prompts/Seed").Text;
				if(int.TryParse(se, out seed) == false)
				{
					System.Random r = new Random();
					seed = r.Next(2147483647);
				}

				string sharkLoc = sharkLocation + "\\shark\\examples\\shark_inference\\stable_diffusion";
				string outputLoc = outputLocation + "sharkOutputs";
				string flags = " --no-progress_bar";
				if (prompt.Trim().Length > 0)
				{
					flags += " --prompts \""+prompt+"\"";
				}
				if(negativePrompt.Trim().Length > 0) { flags += " --negative-prompts \"" + negativePrompt + "\""; }
				flags += " --output_dir \"" + outputLoc + "\"";
				flags += " --seed " + seed.ToString();
				flags += " --runs " + runs;
				flags += " --steps " + steps;

				cmd = new Process();
				cmd.StartInfo.FileName = "cmd.exe";
				cmd.StartInfo.RedirectStandardInput = true;
				cmd.StartInfo.RedirectStandardOutput = true;
				//cmd.StartInfo.CreateNoWindow = true;
				//cmd.StartInfo.UseShellExecute = false;
				cmd.Start();
				cmd.StandardInput.WriteLine(sharkLocation + "\\shark.venv\\Scripts\\activate.bat");
				string COM = "python " + sharkLoc + "\\main.py " + flags;
				GD.Print("Comman string: " + COM);
				cmd.StandardInput.WriteLine(COM);





				cmd.StandardInput.Flush();
				cmd.StandardInput.Close();
				cmd.WaitForExit();


			});
		}
	}
}
