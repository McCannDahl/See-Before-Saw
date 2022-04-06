using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Globalization;

public class WeekDayLayout : DatePickerLayout {


	public override void setupCells(DateTime now, Calendar.ActivePanel activePanel) {
		// get all child objects
		getCells ();

		CultureInfo culture = CultureInfo.CurrentCulture;
		int i = 0;
		foreach (Button go in CalendarCells) {
			go.transform.Find ("Text").GetComponent<Text> ().text = culture.DateTimeFormat.AbbreviatedDayNames[i].ToString ();
			go.transform.Find ("Text").GetComponent<Text> ().fontSize = 22;
			go.transform.Find ("Text").GetComponent<Text> ().color = fontActiveColor;
			go.transform.GetComponent<Image> ().sprite = actualEntryImg;
			i++;
			go.transform.GetChild(1).gameObject.SetActive(false);
		}
	}
	
}
