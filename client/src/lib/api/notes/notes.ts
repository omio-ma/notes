import type { Note } from "../../types";
import agent from "../../agent";

export async function getAllNotes(): Promise<Note[]> {
  const response = await agent.get<Note[]>("/notes");
  return response.data;
}
