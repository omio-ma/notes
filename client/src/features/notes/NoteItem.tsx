import { useState } from "react";
import type { Note } from "../../lib/types";

type Props = {
  note: Note;
  onClick: () => void;
};

function NoteItem({ note, onClick }: Props) {
  return (
    <li
      data-testid={`note-item-${note.id}`}
      onClick={onClick}
      className="p-4 rounded-md border border-orange-300 bg-orange-100 cursor-pointer hover:bg-orange-200 transition-all duration-300"
    >
      <p className="text-lg font-medium text-orange-800">{note.title}</p>
    </li>
  );
}

export default NoteItem;
