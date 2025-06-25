import type { Note } from "../../types";
import agent from "../../agent";

export async function getAllNotes(): Promise<Note[]> {
  const response = await agent.get<Note[]>("/notes");
  return response.data;
}

export async function updateNote(
  id: number,
  note: Omit<Note, "id" | "createdAt">
): Promise<Note> {
  const response = await agent.put<Note>(`/notes/${id}`, note);
  return response.data;
}
