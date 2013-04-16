using UnityEditor;
using UnityEngine;

public class Git : EditorWindow
{

    private class FileToggle
    {
        private string filename;
        private bool toggle;
        public FileToggle(string filename, bool toggle = false)
        {
            this.filename = filename;
            this.toggle = toggle;
        }

        public string GetFilename() 
        {
            return filename;
        }

        public bool GetBool()
        {
            return toggle;
        }

        public void SetBool(bool toggle)
        {
            this.toggle = toggle;
        }
    }

    //wraps git ops
    private class GitAccessor
    {
        //exception for git ops
        public class GitAccessorException : UnityException {

            public GitAccessorException(string message) : base(message) {

            }
        }

        private string gitPath,selectedBranch,selectedRemote; //we don't want to
        //store much, but these are more convienent to store here, then elsewhere

        private System.IO.FileSystemWatcher fs;
        private System.Collections.ArrayList changedFiles;

        //constructor, given the exectuable git path, and a ts
        public GitAccessor(string gitPath) {
            this.gitPath = gitPath;
            this.fs = new System.IO.FileSystemWatcher(".");
            this.fs.Changed += new System.IO.FileSystemEventHandler(this.FileChanged);
            this.fs.Created += new System.IO.FileSystemEventHandler(this.FileChanged);
            this.fs.Deleted += new System.IO.FileSystemEventHandler(this.FileChanged);
            this.fs.Renamed += new System.IO.RenamedEventHandler(this.FileRenamed);
        }

        public void UpdateGitPath(string gitPath)
        {
            this.gitPath = gitPath;
            this.fs = new System.IO.FileSystemWatcher(".");
            this.fs.Changed += new System.IO.FileSystemEventHandler(this.FileChanged);
            this.fs.Created += new System.IO.FileSystemEventHandler(this.FileChanged);
            this.fs.Deleted += new System.IO.FileSystemEventHandler(this.FileChanged);
            this.fs.Renamed += new System.IO.RenamedEventHandler(this.FileRenamed);
        }

        public string GetGitPath()
        {
            return gitPath;
        }

        public string[] GetBranches()
        {
            string[] res = ExecuteCommand("branch");
            for (int i = 0; i < res.Length; i++)
                //foreach (char c in s)
                //Debug.Log((int)res[0][0] +" "+ res[0].Contains("*"));
                res[i] = res[i].TrimStart((char)42, (char)32);
                //res[i] = res[i].Replace((char)42,'\0');
            return res;
            //throw new GitAccessorException("Get Branches Failure");
        }

        public string[] GetRemotes()
        {
            return ExecuteCommand("remote");
            //throw new GitAccessorException("Get Remotes Failure");
        }

        public void AddBranch(string branchName)
        {
            ExecuteCommand("branch", branchName);
            //throw new GitAccessorException("Add Branch Failure");
        }

        public void RemoveBranch(string branchName)
        {
            ExecuteCommand("branch", "--delete", branchName);
            //throw new GitAccessorException("Remove Branch Failure");
        }

        public void AddRemote(string remoteName,string url)
        {
            ExecuteCommand("remote", "add", remoteName, url);
            //throw new GitAccessorException("Add Remote Failure");
        }

        public void RemoveRemote(string remoteName)
        {
            ExecuteCommand("remote", "remove", remoteName);
            //throw new GitAccessorException("Remove Remote Failure");
        }

        public void SelectBranch(string branchName)
        {
            if (GIT_GLOBAL_DEBUG)
                Debug.Log("Selected remote: " + branchName);

            ExecuteCommand("checkout", branchName);
            selectedBranch = branchName;
            //throw new GitAccessorException("Select Branch Failure");
        }

        public void SelectRemote(string remoteName)
        {
            if (GIT_GLOBAL_DEBUG)
                Debug.Log("Selected remote: "+remoteName);
            selectedRemote = remoteName;
        }

        public void Commit(string message = null)
        {
            if (message == null)
                message = "committed at "+System.DateTime.Now.ToString();
            message.Replace("'", "\""); //strip out ' 

            ExecuteCommand("commit","-m '"+message);

            //throw new GitAccessorException("Commit Failure");
        }

        public void Push()
        {
            ExecuteCommand("push", selectedRemote, selectedBranch);
            //throw new GitAccessorException("Push Failure");
        }

        public void Pull()
        {
            ExecuteCommand("pull", selectedBranch, selectedRemote);
            //throw new GitAccessorException("Pull Failure");
        }

        public void Add(params string[] files)
        {
            string[] res = ExecuteCommand("add", files);
            string ex = "";
            foreach (string s in res)
                ex += s + " ";
            Debug.Log(ex);
            throw new GitAccessorException(ex);
        }

        private string[] ExecuteCommand(string command, params string[] arguments)
        {
            string args = " ";
            foreach (string a in arguments)
                args += a + " ";

            System.Collections.ArrayList lines = new System.Collections.ArrayList();
            if (GIT_GLOBAL_DEBUG)
                Debug.Log("starting git process w/ "+command+args);

            System.Diagnostics.Process p = System.Diagnostics.Process.Start(
            new System.Diagnostics.ProcessStartInfo
            {
                FileName = gitPath,
                Arguments = command + args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                
            });
            while (!p.StandardOutput.EndOfStream)
            {
                string line = p.StandardOutput.ReadLine();
                if (GIT_GLOBAL_DEBUG)
                    Debug.Log("response: "+line);
                lines.Add(line);

            }
            if (!p.WaitForExit(60000)) //wait for one minute before
                p.Close(); //closing
            
            return (string[]) lines.ToArray(typeof(string));
        }

        public FileToggle[] GetChangedFiles()
        {
            if (changedFiles == null)
                return null;
            else
                return (FileToggle[])changedFiles.ToArray(typeof(FileToggle)); 
        }

        private void FileChanged(object source, System.IO.FileSystemEventArgs e)
        {
            if (changedFiles == null)
                changedFiles = new System.Collections.ArrayList();

            if (!changedFiles.Contains(new FileToggle(e.FullPath)))
                changedFiles.Add(new FileToggle(e.FullPath));

        }

        private void FileRenamed(object source, System.IO.RenamedEventArgs e)
        {
            if (changedFiles == null)
                changedFiles = new System.Collections.ArrayList();

            if (!changedFiles.Contains(new FileToggle(e.OldFullPath)))
            {
                changedFiles.Remove(new FileToggle(e.OldFullPath));
                changedFiles.Add(new FileToggle(e.FullPath));
            }
                
        }
    }



    private static bool GIT_GLOBAL_DEBUG = false;

    private string GITPATH = "D:/nix/bin/git.exe";
    private GitAccessor gitAccessor; 

    private bool coreSettings = true, branchSettings = true, remoteSettings = true, remoteRescan, branchRescan;
    private int branchIndex = 0,oldBranchIndex = 0,remoteIndex = 0, oldRemoteIndex = 0;
    private int[] branchInts, remoteInts;
    private string[] branchStrings, remoteStrings;
    private string message;
    
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Git")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(Git));
    }

    
    void OnEnable()
    {

        resumeState();

        if (GIT_GLOBAL_DEBUG)
            Debug.Log("GIT: OnEnable Called.");
        
        //create the gitAccessor
        gitAccessor = new GitAccessor(GITPATH);
    }

    void OnFocus()
    {
        if (GIT_GLOBAL_DEBUG)
            Debug.Log("GIT: OnFocus Called.");

        resumeState();

        if (gitAccessor != null)
            gitAccessor.UpdateGitPath(gitAccessor.GetGitPath()); //if we regain focus, dirty verify that fs is constructed

    }

    void OnLostFocus()
    {
        if (GIT_GLOBAL_DEBUG)
            Debug.Log("GIT: OnLostFocus Called.");

        saveState();
    }

    void OnDestroy()
    {
        if (GIT_GLOBAL_DEBUG)
            Debug.Log("GIT: OnDestroy Called.");

        saveState();
    }

    void OnGUI()
    {

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Pull"))
        {
            try
            {
                gitAccessor.Pull();
            }
            catch (GitAccessor.GitAccessorException e)
            {
                message = e.Message.ToString();
            }
        }

        if (GUILayout.Button("Add * - Commit"))
        {
            try
            {
                gitAccessor.Add("*");
                gitAccessor.Commit();
            }
            catch (GitAccessor.GitAccessorException e)
            {
                message = e.Message.ToString();
            }
        }

        if (GUILayout.Button("Push"))
        {
            try
            {
                gitAccessor.Push();
            }
            catch (GitAccessor.GitAccessorException e)
            {
                message = e.Message.ToString();
            }
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.Label(message);
        EditorGUILayout.EndVertical();

        //the files that you've changed
        EditorGUILayout.BeginVertical();
        FileToggle[] files = gitAccessor.GetChangedFiles();
        if (files != null)
            foreach (FileToggle f in files)
                f.SetBool(EditorGUILayout.Toggle(f.GetFilename(), f.GetBool()));
        EditorGUILayout.EndVertical();


        //CORE SETTINGS
        coreSettings = EditorGUILayout.Foldout(coreSettings, "Core Settings");
        if (coreSettings)
        {
            GITPATH = EditorGUILayout.TextField("Git Application:", GITPATH);
            if (!gitAccessor.GetGitPath().Equals(GITPATH))
                gitAccessor.UpdateGitPath(GITPATH);

            GIT_GLOBAL_DEBUG = EditorGUILayout.Toggle("Debug Mode:", GIT_GLOBAL_DEBUG);
        }

        //BRANCH SETTINGS
        branchSettings = EditorGUILayout.Foldout(branchSettings, "Branch Settings");
        if (branchSettings)
        {
            if (!branchRescan)
            {
                branchRescan = true;
                branchStrings = gitAccessor.GetBranches();
                branchInts = new int[branchStrings.Length];
                for (int i = 0; i < branchInts.Length; i++)
                    branchInts[i] = i;
            }

            if (branchStrings != null && branchInts != null)
                if (branchInts.Length > 0 && branchStrings.Length > 0)
                    if (branchInts.Length == branchStrings.Length)
                    {
                        if (branchInts.Length == 1)
                        {
                            gitAccessor.SelectBranch(branchStrings[0]);
                            branchIndex = 0;
                        }

                        oldBranchIndex = branchIndex;
                        branchIndex = EditorGUILayout.IntPopup("Branch: ", branchIndex, branchStrings, branchInts);
                        if (oldBranchIndex != branchIndex)
                            gitAccessor.SelectBranch(branchStrings[branchIndex]);
                    }
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Reload"))
            {
                branchStrings = gitAccessor.GetBranches();
                branchInts = new int[branchStrings.Length];
                for (int i = 0; i < branchInts.Length; i++)
                    branchInts[i] = i;
            }
            if (GUILayout.Button("Remove"))
                gitAccessor.RemoveBranch(branchStrings[branchIndex]);
            /*if (GUILayout.Button("Create"))
                GUILayout.TextField(
                addBranch();
            */

            EditorGUILayout.EndHorizontal();
        }
        else
            if (branchRescan)
                branchRescan = false;

        //REMOTE SETTINGS
        remoteSettings = EditorGUILayout.Foldout(remoteSettings, "Remote Settings");
        if (remoteSettings)
        {
            if (!remoteRescan)
            {
                remoteRescan = true;
                remoteStrings = gitAccessor.GetRemotes();
                remoteInts = new int[remoteStrings.Length];
                for (int i = 0; i < remoteInts.Length; i++)
                    remoteInts[i] = i;
            }

            if (remoteStrings != null && remoteInts != null)
                if (remoteInts.Length > 0 && remoteStrings.Length > 0)
                    if (remoteInts.Length == remoteStrings.Length)
                    {
                        if (remoteInts.Length == 1)
                        {
                            gitAccessor.SelectRemote(remoteStrings[0]);
                            remoteIndex = 0;
                        }

                        oldRemoteIndex = remoteIndex;
                        branchIndex = EditorGUILayout.IntPopup("Remote: ", branchIndex, remoteStrings, remoteInts);
                        if (oldRemoteIndex != remoteIndex)
                            gitAccessor.SelectRemote(remoteStrings[remoteIndex]);
                    }
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Reload"))
            {
                remoteStrings = gitAccessor.GetRemotes();
                remoteInts = new int[remoteStrings.Length];
                for (int i = 0; i < remoteInts.Length; i++)
                    remoteInts[i] = i;
            }
            if (GUILayout.Button("Remove"))
                gitAccessor.RemoveRemote(remoteStrings[remoteIndex]);
            /*if (GUILayout.Button("Create"))
                GUILayout.TextField(
                addBranch();
            */

            EditorGUILayout.EndHorizontal();
        }
        else
            if (remoteRescan)
                remoteRescan = false;

       
    }

    private void resumeState()
    {
        //deserialize from reg
        if (EditorPrefs.HasKey("git-GITPATH"))
            GITPATH = EditorPrefs.GetString("git-GITPATH");

        if (EditorPrefs.HasKey("git-GIT_GLOBAL_DEBUG"))
            GIT_GLOBAL_DEBUG = EditorPrefs.GetBool("git-GIT_GLOBAL_DEBUG");

        if (EditorPrefs.HasKey("git-coreSettings"))
            coreSettings = EditorPrefs.GetBool("git-coreSettings");

        if (EditorPrefs.HasKey("git-branchSettings"))
            branchSettings = EditorPrefs.GetBool("git-branchSettings");

        if (EditorPrefs.HasKey("git-remoteSettings"))
            remoteSettings = EditorPrefs.GetBool("git-remoteSettings");

    }

    private void saveState()
    {

        //remove stale keys
        if (EditorPrefs.HasKey("git-GITPATH"))
            EditorPrefs.DeleteKey("git-GITPATH");

        if (EditorPrefs.HasKey("git-GIT_GLOBAL_DEBUG"))
            EditorPrefs.DeleteKey("git-GIT_GLOBAL_DEBUG");

        if (EditorPrefs.HasKey("git-coreSettings"))
            EditorPrefs.DeleteKey("git-coreSettings");

        if (EditorPrefs.HasKey("git-branchSettings"))
            EditorPrefs.DeleteKey("git-branchSettings");

        if (EditorPrefs.HasKey("git-remoteSettings"))
            EditorPrefs.DeleteKey("git-remoteSettings");

        //serialize new keys
        EditorPrefs.SetString("git-GITPATH", GITPATH);

        EditorPrefs.SetBool("git-GIT_GLOBAL_DEBUG",GIT_GLOBAL_DEBUG);

        EditorPrefs.SetBool("git-coreSettings",coreSettings);

        EditorPrefs.SetBool("git-branchSettings",branchSettings);

        EditorPrefs.SetBool("git-remoteSettings",remoteSettings);

    }

}