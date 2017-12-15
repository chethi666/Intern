package robo;

import java.util.ArrayList;
import java.util.List;
import org.json.JSONArray;
import org.json.JSONObject;
import com.sun.jersey.api.client.Client;
import com.sun.jersey.api.client.ClientResponse;
import com.sun.jersey.api.client.WebResource;

public class RetrieveRobots { /// Retrieve Robots from server

	public List<String> getRobots() {

		List<String> Robo = new ArrayList<String>();///get available robots

		try {

			Client client = Client.create();

			WebResource webResource = client
					.resource("http://checkmate.ukwest.cloudapp.azure.com:9100/documentServer/mongo/EntityCard?query="
							+ "_schema:WebSource&itemsPerPage=50");

			ClientResponse response = webResource.accept("application/json").get(ClientResponse.class);

			if (response.getStatus() != 200) {
				throw new RuntimeException("Failed : HTTP error code : " + response.getStatus());

			}

			String output = response.getEntity(String.class);
			JSONObject jsonObj = new JSONObject(output);

			int length = jsonObj.getInt("DocumentCount");/// lenght of "DocumentCount" array object in JSON
			JSONArray DocumentArray = jsonObj.getJSONArray("Documents");

			for (int i = 0; i < length; i++) {

				JSONObject firstDocument = DocumentArray.getJSONObject(i);
				String url = firstDocument.getString("sourceUrl");//// get Urls of available robots

				if (url.trim() != null) {

					Robo.add(url);//// ///ADDING URLs
				}
			}

		} catch (Exception ex) {

		}

		return Robo;
	}
}
