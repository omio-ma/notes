import { render, screen } from "@testing-library/react";
import NotesList from "./NotesList";
import type { Note } from "../../lib/types";

describe("NotesList", () => {
  it("renders the correct number of NoteItem components", () => {
    const mockNotes: Note[] = [
      {
        id: 1,
        title: "Note 1",
        content: "Content 1",
        createdAt: new Date().toISOString()
      },
      {
        id: 2,
        title: "Note 2",
        content: "Content 2",
        createdAt: new Date().toISOString()
      },
      {
        id: 3,
        title: "Note 3",
        content: "Content 3",
        createdAt: new Date().toISOString()
      }
    ];

    render(<NotesList notes={mockNotes} />);

    mockNotes.forEach((note) => {
      expect(screen.getByTestId(`note-item-${note.id}`)).toBeDefined();
    });
  });
});
