import { fireEvent, render, screen } from "@testing-library/react";
import NoteForm from "./NoteForm";
import type { Note } from "../../lib/types";

describe("NoteForm", () => {
  const mockNote: Note = {
    id: 1,
    title: "Original Title",
    content: "Original Content",
    createdAt: new Date().toISOString()
  };

  it("updates input fields when typing", () => {
    const mockOnSave = jest.fn();
    const mockOnBack = jest.fn();

    render(
      <NoteForm note={mockNote} onSave={mockOnSave} onBack={mockOnBack} />
    );

    const titleInput = screen.getByTestId("note-form-title");
    const contentTextarea = screen.getByTestId("note-form-content");

    fireEvent.change(titleInput, { target: { value: "Updated Title" } });
    fireEvent.change(contentTextarea, { target: { value: "Updated Content" } });

    fireEvent.click(screen.getByTestId("note-form-save-button"));

    expect(mockOnSave).toHaveBeenCalledWith({
      ...mockNote,
      title: "Updated Title",
      content: "Updated Content"
    });
  });

  it("calls onBack when Back button is clicked", () => {
    const mockOnBack = jest.fn();

    render(<NoteForm note={mockNote} onSave={jest.fn()} onBack={mockOnBack} />);

    fireEvent.click(screen.getByTestId("note-form-back-button"));

    expect(mockOnBack).toHaveBeenCalled();
  });
});
