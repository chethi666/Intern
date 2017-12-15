package robo;

import java.util.TimerTask;

import org.json.JSONException;
import org.json.simple.parser.ParseException;



// Create a class extends with TimerTask
public  class ScheduledTask extends TimerTask {////scheduled task

	CheckStatus check=new CheckStatus();

	public void run() {
		
		
		
		try {
			check.Check();///Task is starting here
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (ParseException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}