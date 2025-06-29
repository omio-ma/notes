import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createNote } from "../../api/notes/notes";
import type { Note } from "../../types";

export function useCreateNote() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: Omit<Note, "id" | "createdAt">) => createNote(data),

    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["notes"] });
    }
  });
}
