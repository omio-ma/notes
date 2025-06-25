import { useUpdateNote } from "../../lib/hooks/notes/useUpdateNote";
import type { Note } from "../../lib/types";
import NoteForm from "./NoteForm";
import NoteItem from "./NoteItem";

type Props = {
  notes: Note[] | undefined;
  selectedNote: Note | null;
  setSelectedNote: (note: Note | null) => void;
};

function NotesList({ notes, selectedNote, setSelectedNote }: Props) {
  const { mutate } = useUpdateNote();

  if (selectedNote) {
    return (
      <NoteForm
        note={selectedNote}
        onBack={() => setSelectedNote(null)}
        onSave={(updatedNote) => {
          mutate(
            {
              id: updatedNote.id,
              data: {
                title: updatedNote.title,
                content: updatedNote.content
              }
            },
            {
              onSuccess: () => setSelectedNote(null)
            }
          );
        }}
      />
    );
  }

  return (
    <ul className="space-y-4 transition-all duration-300">
      {notes?.map((note) => (
        <NoteItem
          key={note.id}
          note={note}
          onClick={() => setSelectedNote(note)}
        />
      ))}
    </ul>
  );
}

export default NotesList;
