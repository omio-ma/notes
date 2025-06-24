import type { Note } from "../../lib/types";
import NoteItem from "./NoteItem";

type Props = {
  notes: Note[] | undefined;
};

function NotesList({ notes }: Props) {
  return (
    <ul className="space-y-4">
      {notes?.map((note: Note) => (
        <NoteItem note={note} key={note.id} />
      ))}
    </ul>
  );
}

export default NotesList;
