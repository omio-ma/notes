import { useQuery } from "@tanstack/react-query";
import { getAllNotes } from "../../api/notes/notes";
import type { Note } from "../../types";

export function useNotes() {
  return useQuery<Note[]>({
    queryKey: ["notes"],
    queryFn: getAllNotes
  });
}
