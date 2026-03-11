const BASE_URL = "https://localhost:7001/api";

export interface Client {
  id: string;
  name: string;
  apiKey: string;
  createdAt: string;
}

export async function createClient(name: string): Promise<Client> {
  const res = await fetch(`${BASE_URL}/clients`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ name }),
  });

  return res.json();
}

export async function getClients(): Promise<Client[]> {
  const res = await fetch(`${BASE_URL}/clients`);
  return res.json();
}

export async function setRateLimit(
  clientId: string,
  requestsPerMinute: number,
) {
  await fetch(`${BASE_URL}/ratelimits`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      clientId,
      requestsPerMinute,
    }),
  });
}

export async function callTest(apiKey: string): Promise<string> {
  const res = await fetch(`${BASE_URL}/test`, {
    headers: {
      "X-API-KEY": apiKey,
    },
  });

  return res.text();
}
