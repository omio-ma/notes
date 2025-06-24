import type { Note } from "../../lib/types";

type Props = {
  note: Note;
};

function NoteItem({ note }: Props) {
  return (
    <li
      data-testid={`note-item-${note.id}`}
      className="p-4 rounded-md border border-orange-200 bg-orange-50"
    >
      <p className="text-lg font-medium text-orange-800">{note.title}</p>
    </li>
  );
}

export default NoteItem;
