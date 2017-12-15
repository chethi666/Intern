package robo;

import java.util.logging.Logger;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.simple.parser.ParseException;
import com.sun.jersey.api.client.Client;
import com.sun.jersey.api.client.ClientResponse;
import com.sun.jersey.api.client.WebResource;

public class CheckStatus {    ///// Check Daily status of robots (Starting from here)
	final static Logger logger = Logger.getLogger("CheckStatus.class");

	public void Check() throws JSONException, ParseException {

		RetrieveRobots rtv = new RetrieveRobots();
		List<String> Robo = rtv.getRobots();//// List of available robots in server

		for (String line : Robo) {

			DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
			Date date = new Date();
			String today = dateFormat.format(date).substring(0, 10).replaceAll("[/]", "-"); ////SET DATE TO TODAY
			String yesterday = dateFormat.format(yesterday()).substring(0, 10).replaceAll("[/]", "-");////SET DATE TO YESTERDAY

			try {

				Client client = Client.create();

				WebResource webResource1 = client.resource(
						"http://checkmate.ukwest.cloudapp.azure.com:9100/documentServer/documentRepository/documents?query=SourceUrl%3A%22"
								+ line + "%22PublishDate%3A%5B" + yesterday + "+TO+" + today
								+ "%5D&sortDescending=true");

				ClientResponse response = webResource1.accept("application/json").get(ClientResponse.class);

				if (response.getStatus() != 200) {
					throw new RuntimeException("Failed : HTTP error code : " + response.getStatus());

				}

				String output = response.getEntity(String.class);

				JSONObject jsonObj = new JSONObject(output);

				JSONArray DocumentArray = jsonObj.getJSONArray("Documents");

				JSONObject firstDocument = DocumentArray.getJSONObject(0);///check whether that Data has Text object of Document JSON array 
				String contentText = firstDocument.getString("Text");

				if (contentText != null) {

					logger.info("This          " + line + "================Robot is working");

				} else {
					logger.warning("This             " + line + "================Robot is NOT working");

				}

			} catch (Exception e) {
			}

		}
	}

	private Date yesterday() {
		final Calendar cal = Calendar.getInstance(); //// set the Yesterday date
		cal.add(Calendar.DATE, -1);
		return cal.getTime();
	}

}