import { useState } from "react";
import type { Note } from "../../lib/types";

type Props = {
  note: Note;
  onBack: () => void;
  onSave: (note: Note) => void;
};

function NoteForm({ note, onBack, onSave }: Props) {
  const [title, setTitle] = useState(note.title);
  const [content, setContent] = useState(note.content);

  return (
    <form
      onSubmit={(e) => {
        e.preventDefault();
        onSave({ ...note, title, content });
      }}
      className="space-y-4 animate-fade-in"
    >
      <input
        type="text"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        className="w-full p-2 border border-gray-300 rounded"
        placeholder="Title"
        data-testid="note-form-title"
      />
      <textarea
        value={content}
        onChange={(e) => setContent(e.target.value)}
        className="w-full p-2 border border-gray-300 rounded"
        rows={5}
        placeholder="Content"
        data-testid="note-form-content"
      />
      <div className="flex justify-between">
        <button
          type="button"
          onClick={onBack}
          className="px-4 py-2 bg-gray-300 text-gray-800 rounded cursor-pointer"
          data-testid="note-form-back-button"
        >
          Back
        </button>
        <button
          data-testid="note-form-save-button"
          type="submit"
          className="px-4 py-2 bg-green-600 text-white rounded cursor-pointer"
        >
          Save
        </button>
      </div>
    </form>
  );
}

export default NoteForm;
