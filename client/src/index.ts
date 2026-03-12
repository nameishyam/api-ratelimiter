const BASE_URL = "http://localhost:5295/api";

export interface Client {
  id: string;
  name: string;
  apiKey: string;
  createdAt: string;
}

async function handleResponse(res: Response) {
  if (!res.ok) {
    const error = await res.text();
    throw new Error(error || res.statusText);
  }
  return res;
}

export async function createClient(name: string): Promise<Client> {
  const res = await fetch(`${BASE_URL}/clients`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ name }),
  }).then(handleResponse);
  return res.json();
}

export async function getClients(): Promise<Client[]> {
  const res = await fetch(`${BASE_URL}/clients`).then(handleResponse);
  return res.json();
}

export async function setRateLimit(
  clientId: string,
  requestsPerMinute: number,
) {
  await fetch(`${BASE_URL}/ratelimits`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ clientId, requestsPerMinute }),
  }).then(handleResponse);
}

export async function callTest(
  apiKey: string,
): Promise<{ status: number; text: string }> {
  const res = await fetch(`${BASE_URL}/test`, {
    headers: { "X-API-KEY": apiKey },
  });
  const text = await res.text();
  return { status: res.status, text };
}
