package robo;

import java.util.*;
import org.json.simple.parser.ParseException;
import org.json.JSONException;

public class Service {

	public static void main(String[] args) throws JSONException, ParseException, InterruptedException {

		Timer time = new Timer(); // Instantiate Timer Object
		ScheduledTask st = new ScheduledTask(); // Instantiate SheduledTask class
		time.schedule(st, 0, 10000); // Create Repetitively task for every 10 sec

	}

}
