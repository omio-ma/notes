import { useMutation, useQueryClient } from "@tanstack/react-query";
import { updateNote } from "../../api/notes/notes";
import type { Note } from "../../types";

export function useUpdateNote() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      data
    }: {
      id: number;
      data: Omit<Note, "id" | "createdAt">;
    }) => updateNote(id, data),

    onSuccess: (updatedNote) => {
      queryClient.setQueryData<Note[]>(["notes"], (oldNotes) =>
        oldNotes?.map((note) =>
          note.id === updatedNote.id ? updatedNote : note
        )
      );
    }
  });
}
