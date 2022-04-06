using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectStatusController : MonoBehaviour
{
    public GameObject project;
    ProjectController script;
    Material statusLight;
    TextMesh statusText;
    private void Awake()
    {
        script = project.GetComponent<ProjectController>();
        script.TasksUpdated += (_) => UpdateTaskString();
        EventPublisher.i.ChangePermissions += (_) => UpdateTaskString();
        statusLight = transform.Find("StatusLight").gameObject.GetComponent<Renderer>().material;
        statusText = transform.Find("StatusText").gameObject.GetComponent<TextMesh>();
    }
    private void Start()
    {
        UpdateTaskString();
    }

    void UpdateTaskString()
    {
        List<ProjectTask> mytasksIncomplete = script.getIncompleteTasks();
        // get my incomplete tasks that have dependancies complete
        List<ProjectTask> mytasksThatHaveCompleteDependancies = script.getIncompleteTasksWithCompleteDependancies().ToList();
        statusText.text = "";
        if (mytasksIncomplete.Count == 0)
        {
            // set light green
            statusLight.SetColor("_EmissionColor", Color.green);
        }
        else if (mytasksThatHaveCompleteDependancies.Count == 0)
        {
            // set light yellow
            statusLight.SetColor("_EmissionColor", Color.yellow);
        }
        else if (mytasksThatHaveCompleteDependancies.Count > 0)
        {
            // set light red
            statusLight.SetColor("_EmissionColor", Color.red);
            statusText.text = mytasksThatHaveCompleteDependancies[0].name;
        }
    }
}
