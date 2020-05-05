import * as signalR from "@microsoft/signalr";
const { VUE_APP_API_HOST, VUE_APP_API_PORT } = process.env;


const createHubConnection = ((): signalR.HubConnection => {
  return new signalR.HubConnectionBuilder()
  .withUrl(`https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/Hubs/Game`)
  .build();
})

export { createHubConnection };
